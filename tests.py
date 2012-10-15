import unittest
import sys
import os
sys.path.append(os.getcwd() + '\\src\\bin')

import System
import clr
clr.AddReferenceByPartialName("System.Core")

clr.AddReferenceByPartialName("System.Configuration")
from System.Configuration import *

from System.Collections import IDictionary
from System.Collections.Generic import IDictionary, IList

clr.AddReferenceToFile('App.dll') 
import App.Core.Models as Models

import App.Core.DataAccess as DataAccess
class DataAccessTests(unittest.TestCase):
    CONN_NAME = "Northwind"
    CONN_STRING = "Server=localhost;Database=northwinddb;Uid=springqa;Pwd=springqa;Convert Zero Datetime=true;Allow Zero Datetime=false"
    CONN_PROVIDER = "MySql.Data.MySqlClient"

    def test_can_get_a_customer_from_db(self):
        dao = DataAccess.CustomersDao(self.CONN_NAME, self.CONN_STRING, self.CONN_PROVIDER)
        customer = dao.Get('ALFKI')
        self.assertTrue(isinstance(customer, Models.Customer))
        self.assertEqual('ALFKI', customer.CustomerId)
        
    def test_can_get_all_customers_from_db(self):
        dao = DataAccess.CustomersDao(self.CONN_NAME, self.CONN_STRING, self.CONN_PROVIDER)
        customers = dao.GetAll()
        self.assertNotEqual(None, customers)
        self.assertTrue(isinstance(customers, IList[Models.Customer]))
        self.assertNotEqual(0, customers.Count)
        
    def test_can_get_all_orders_for_customer_from_db(self):
        dao = DataAccess.OrdersDao(self.CONN_NAME, self.CONN_STRING, self.CONN_PROVIDER)
        orders = dao.GetAllForCustomer('ALFKI')
        self.assertNotEqual(None, orders)
        self.assertTrue(isinstance(orders, IList[Models.Order]))
        self.assertNotEqual(0, orders.Count)

import App.Core.Services as Services
from unittest.Mock import MagicMock
class ServicesTests(unittest.TestCase):
    def test_can_ship_order_for_customer(self):
        fullfillmentSvc = Services.FullfillmentService()
        fullfillmentSvc.ProcessCustomer = MagicMock(name='ProcessCustomer',return_value=None)
                
    def test_can_process_customer(self):
        pass
        
    def test_can_get_customers(self):
        pass

clr.AddReferenceByPartialName("System.Web.Mvc")
from System.Web.Mvc import *
import App.Controllers as Controllers
class ControllerTests(unittest.TestCase):
    pass

if __name__ == '__main__':
    unittest.main()