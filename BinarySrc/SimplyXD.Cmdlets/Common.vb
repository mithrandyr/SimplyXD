Imports System.Linq.Expressions

Module Common
    Public Property DefaultTimeout As Integer = Common.DefaultTimeout

    'ReadOnly Property PortalURI As String
    '    Get
    '        Return
    '    End Get
    'End Property

    'Function XDPortal() As PortalODataContext
    '    If _CurrentXDPortal Is Nothing Then
    '        Throw New InvalidOperationException("No Xpertdoc Portal connected, please use 'Connect-XdPortal'.")
    '    Else
    '        Return New PortalODataContext(_CurrentXDPortal) With {.Credentials = Net.CredentialCache.DefaultCredentials, .MergeOption = Microsoft.OData.Client.MergeOption.NoTracking}
    '    End If
    'End Function

    <Runtime.CompilerServices.Extension()>
    Public Function WherePropertyIsIn(Of T, TSet)(ByVal query As IQueryable(Of T), ByVal valuesList As IEnumerable(Of TSet), ByVal propertySelector As Expression(Of Func(Of T, TSet))) As IQueryable(Of T)
        If valuesList Is Nothing Then Throw New ArgumentNullException(NameOf(valuesList))

        'if there are no values, no entities can fullfil the condition -> return empty
        If Not valuesList.Any() Then Return Enumerable.Empty(Of T)().AsQueryable()

        'create a check for each value
        Dim filters = valuesList.Select(Function(value) Expression.Equal(propertySelector.Body, Expression.Constant(value)))

        'build an expression aggregating checks with OR, use first check as starter
        Dim firstCheck = filters.First()

        'we could duplicate first check, but why not just skip it
        Dim filterPredicate = filters.Skip(1).Aggregate(firstCheck, Function(x, y) Expression.Or(x, y))

        Dim filterLambdaExpression = Expression.Lambda(Of Func(Of T, Boolean))(filterPredicate, propertySelector.Parameters.Single())
        Return query.Where(filterLambdaExpression)
    End Function

End Module

