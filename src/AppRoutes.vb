Imports System.Web.Routing
Imports System.Web.Mvc

Public Class AppRoutes
    Shared Sub MapRoutes(routes As RouteCollection)
        routes.MapRoute(
            "HomeRoute",
            "Home",
            New With {.controller = "Home", .action = "Index"}
        )
        routes.MapRoute(
            "AboutRoute",
            "About",
            New With {.controller = "Home", .action = "About"}
        )
        routes.MapRoute(
            "CustomersRoute",
            "Customers",
            New With {.controller = "Customers", .action = "Index"}
        )
        routes.MapRoute(
            "CustomersListRoute",
            "Customers/List",
            New With {.controller = "Customers", .action = "List"}
        )
        routes.MapRoute(
            "CustomersGridRoute",
            "Customers/FillCustomersGrid",
            New With {.controller = "Customers", .action = "FillCustomersGrid"}
        )
        routes.MapRoute(
            "CustomersBrowseRoute",
            "Customers/Browse/{customerId}",
            New With {.controller = "Customers", .action = "Browse", .customerId = String.Empty}
        )
        routes.MapRoute(
            "CustomersEditRoute",
            "Customers/Edit/{customerId}",
            New With {.controller = "Customers", .action = "Edit", .customerId = String.Empty}
        )
        routes.MapRoute(
            "OrdersProcesssRoute",
            "Orders/Process/{customerId}/{contactName}",
            New With {.controller = "Orders", .action = "Process", .customerId = String.Empty, .contactName = String.Empty}
        )
        routes.MapRoute(
            "OrdersGridRoute",
            "Orders/FillOrdersGrid/{customerId}",
            New With {.controller = "Orders", .action = "FillOrdersGrid", .customerId = String.Empty}
        )
        routes.MapRoute(
            "DefaultRoute",
            "{controller}/{action}",
            New With {.controller = "Home", .action = "Index"}
        )
    End Sub
End Class
