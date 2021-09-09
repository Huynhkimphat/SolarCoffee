using SolarCoffee.Data.Models;
using SolarCoffee.Web.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SolarCoffee.Web.Serialization
{
	/// <summary>
	/// Handles mapping Order data models to and from View Models
	/// </summary>
	public static class OrderMapper
	{
		/// <summary>
		/// Maps on InvoiceModel view model to a SalesOrder data model
		/// </summary>
		/// <param name="invoice"></param>
		/// <returns></returns>
		public static SalesOrder SerializeInvoiceToOrder(InvoiceModel invoice)
		{
			var saleOrderItems = invoice.LineItems
				.Select(item => new SalesOrderItem {
					Id = item.Id,
					Quantity = item.Quantity,
					Product = ProductMapper.SerializeProductModel(item.Product),
				}).ToList() ;
			return new SalesOrder
			{
				SalesOrderItems = saleOrderItems,
				CreateOn = DateTime.UtcNow,
				UpdateOn = DateTime.UtcNow,
			};
		}

		/// <summary>
		/// Maps a collection of SalesOrders (data) to OrderModels (view models)
		/// </summary>
		/// <param name="orders"></param>
		/// <returns></returns>
		public static List<OrderModel> SerializeOrderToViewModels(IEnumerable<SalesOrder> orders)
		{
			return orders.Select(order=> new OrderModel
			{
				Id=order.Id,
				CreateOn=order.CreateOn,
				UpdateOn=order.UpdateOn,
				SalesOrderItems=SerializeSalesOrderItems(order.SalesOrderItems),
				Customer=CustomerMapper.SerializeCustomer(order.Customer),
				IsPaid=order.IsPaid
			}).ToList();
		}

		/// <summary>
		/// Maps a collection of SalesOrderItems (data) to SalesOrderItemsModels (view models)
		/// </summary>
		/// <param name="orderItems"></param>
		/// <returns></returns>
		private static List<SalesOrderItemModel> SerializeSalesOrderItems(IEnumerable<SalesOrderItem> orderItems)
		{
			return orderItems.Select(item => new SalesOrderItemModel {
				Id=item.Id,
				Quantity=item.Quantity,
				Product=ProductMapper.SerializeProductModel(item.Product)
			}).ToList();
		}
	}
}