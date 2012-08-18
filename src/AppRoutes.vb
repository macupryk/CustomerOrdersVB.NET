Imports System.Web.Routing
Imports System.Web.Mvc

Public Class AppRoutes
    Shared Sub MapRoutes(routes As RouteCollection)
        routes.MapRoute(
            "OrdersProcesssRoute",
            "Orders/Process/{customerId}/{customerName}",
            New With {.controller = "Orders", .action = "Process", .customerId = String.Empty, .contactName = String.Empty}
        )

        routes.MapRoute(
            "CustomersEditRoute",
            "Customers/Edit/{customerId}/{customerName}",
            New With {.controller = "Customers", .action = "Edit", .customerId = String.Empty, .contactName = String.Empty}
        )

        routes.MapRoute(
            "CustomersBrowseRoute",
            "Customers/Browse/{customerId}",
            New With {.controller = "Customers", .action = "Browse", .customerId = String.Empty}
        )

        routes.MapRoute(
            "CustomersListRoute",
            "Customers/List",
            New With {.controller = "Customers", .action = "List"}
        )

        routes.MapRoute(
            "Customers",
            "Customers",
            New With {.controller = "Customers", .action = "Index"}
        )

        routes.MapRoute(
            "AboutRoute",
            "About",
            New With {.controller = "Home", .action = "About"}
        )

        routes.MapRoute(
            "HomeRoute",
            "Home",
            New With {.controller = "Home", .action = "Index"}
        )

        routes.MapRoute(
            "DefaultRoute",
            "{controller}/{action}",
            New With {.controller = "Home", .action = "Index"}
        )
    End Sub
End Class
