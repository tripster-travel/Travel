﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Text;
using Travel.Api.Models;

namespace Travel.Api.Models.Controllers
{
	[RoutePrefix("api/bookings")]
	public class BookingsController : ApiController
	{
		/// Create Booking
		[HttpPost, Route(""), TokenAuthentication(), Authorize(Roles = "api.update")]
		public List<Booking> Create(Booking newBooking)
		{
			// create tickets & barcodes
			foreach (var item in newBooking.Items)
			{
				if (item.ProductNumber == 1001 && item.Quantity == 1)
				{
					// create test ticket
					item.Tickets.Add(new Ticket()
					{
						TicketNumber = "40311219705414559359",
						Barcode = Guid.NewGuid().ToString(),
						Instructions = "1. PRESENT THIS E-TICKET AT THE MAIN ENTRANCE: This e-ticket contains a unique barcode valid for one entry only to Canada’s Wonderland on the date specified on this ticket. Valid for everyone aged 3 years and over 2. GO DIRECTLY TO THE TURNSTILES AT THE FRONT GATE: Please note that you do not need to go to the Admission Sales windows or Guest Services windows. ENJOY YOUR DAY AT CANADA’S WONDERLAND",
						Terms = "Not valid in conjunction with any other discount offer. Not for resale. Cedar Fair reserves the right to refuse admission and control occupancy. Holder assumes all risk of personal injury and loss of property, agrees to the posted conditions of usage, rider safety guidelines and ride admission policies and grants Cedar Fair permission to use his/her photograph or video image in any advertising and promotion without payment to holder. No rain checks, refunds, exchanges or replacements if lost or stolen. Not valid for separately ticketed events or special events. Not valid for Halloween Haunt or WinterFest. Some attractions require an additional charge. Height and physical restrictions may apply on some rides. Please visit the park’s website or call the park to confirm operating dates and hours and learn more about park rules, ride admission policies and rider safety guidelines. Picnic baskets and coolers are welcome at the pavilion located outside Wonderland near the Main Entrance. With the exception of plastic water bottles, outside food and beverages are not allowed in the park"
					});
				}
			}
			newBooking.Status = "Completed";
			newBooking.OrderId = string.Format("TEST-{0:0000}", TestData.Current.Bookings.Count + 1);

			// add new booking
			TestData.Current.Bookings.Add(newBooking);
									
			return new List<Booking>() { newBooking };
		}

		/// Cancel Booking
		[HttpPost, Route("{orderId}/cancel"), TokenAuthentication(), Authorize(Roles = "api.update")]
		public List<Booking> Cancel(string orderId)
		{
			// find booking
			var booking = TestData.Current.Bookings.Find(x => x.OrderId == orderId);

			// return 404 if not found
			if (booking == null) throw new HttpResponseException(HttpStatusCode.NotFound);

			// change booking status
			booking.Status = "Cancelled";

			return new List<Booking>() { booking };
		}

		/// Get Booking
		[HttpGet, Route("{orderId}"), TokenAuthentication(), Authorize(Roles = "api.access")]
		public List<Booking> Detail(string orderId)
		{
			// find booking
			var  booking = TestData.Current.Bookings.Find(x => x.OrderId == orderId);

			// return 404 if not found
			if (booking == null) throw new HttpResponseException(HttpStatusCode.NotFound);

			return new List<Booking>() { booking };
		}

		/// Get All Bookings
		[HttpGet, Route(""), TokenAuthentication(), Authorize(Roles = "api.access")]
		public List<Booking> List()
		{
			// find booking
			var list = TestData.Current.Bookings;

			return list;
		}
	}
}

