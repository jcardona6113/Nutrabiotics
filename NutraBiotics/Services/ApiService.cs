namespace NutraBiotics.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Models;
    using Newtonsoft.Json;
    using System.Linq;
    using NutraBiotics.ViewModels;

    public class ApiService
    {


        public async Task<Response> Login(
            string urlBase,
            string controller,
            string email,
            string password
            )
        {
            try
            {
                var loginRequest = new LoginRequest
                {
                    Email = email,
                    Password = password
                };

                var body = JsonConvert.SerializeObject(loginRequest);
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var response = await client.PostAsync(controller, content);
                var result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<Response>(result);
                    return new Response
                    {
                        IsSuccess = false,
                        Message = error.Message,
                    };
                }

                var user = JsonConvert.DeserializeObject<User>(result);
                return new Response
                {
                    IsSuccess = true,
                    Result = user,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> PostMaster(
        string urlBase,
        string controller,
        SyncShiptoRequest shiptorequest
            )
        {
            try
            {

                var body = JsonConvert.SerializeObject(shiptorequest);
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var response = await client.PostAsync(controller, content);
                var result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<Response>(result);
                    return new Response
                    {
                        IsSuccess = false,
                        Message = error.Message,
                    };
                }

                var shipto = JsonConvert.DeserializeObject<SyncShiptoRequest>(result);
                return new Response
                {
                    IsSuccess = true,
                    Result = shipto,
                };
            }

            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }


        public async Task<Response> GetList<T>(
            string urlBase,
            string controller)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var response = await client.GetAsync(controller);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "No se pudo descargar.",
                    };
                }

                var answer = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<T>>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> SyncOrder(
            string urlBase,
            string controller,
            List<OrderHeader> orders)
        {
            try
            {
                var syncHeaders = new List<SynOrderHeaderRequest>();
                foreach (var order in orders)
                {
                    var syncDetails = new List<SynOrderDetailRequest>();
                    foreach (var detail in order.OrderDetails)
                    {
                        syncDetails.Add(new SynOrderDetailRequest
                        {
                            OrderLine = detail.OrderLine,
                            OrderQty = detail.OrderQty,
                            PartId = detail.PartId,
                            PriceListPartId = detail.PriceListPartId,
                            PartNum = detail.PartNum,
                            Reference = string.Empty,
                            TaxAmt = detail.TaxAmt,
                            UnitPrice = detail.UnitPrice,
                            Total = detail.Total,
                        });
                    }

                    syncHeaders.Add(new SynOrderHeaderRequest
                    {
                        SalesOrderHeaderPhoneId = order.SalesOrderHeaderId,
                        OrderNum = order.OrderNum,
                        UserId = order.UserId,
                        VendorId = order.VendorId,
                        ContactId = order.ContactId,
                        CustId = order.CustId,
                        CreditHold = order.CreditHold,
                        CustomerId = order.CustomerId,
                        Date = order.Date,
                        NeedByDate = order.NeedByDate,
                        ConNum = order.ConNum,
                        ShipToNum = order.ShipToNum,
                        Observations = order.Observations,
                        OrderDetails = syncDetails,
                        SalesCategory = order.SalesCategory,
                        ShipToId = order.ShipToId,
                        TermsCode = order.TermsCode,
                        IsSync = order.IsSync,
                        Total = order.Total,
                        SincronizadoEpicor = order.SincronizadoEpicor,
                        SalesOrderHeaderInterId = order.SalesOrderHeaderInterId,
                        RowMod = order.RowMod,
                    });
                }

                var request = JsonConvert.SerializeObject(syncHeaders);
                var content = new StringContent(request, Encoding.UTF8, "application/json");

                var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(50000);
                client.BaseAddress = new Uri(urlBase);
                var response = await client.PostAsync(controller, content);
                var result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<Response>(result);
                    return new Response
                    {
                        IsSuccess = false,
                        Message = error.Message,
                    };
                }

                var answer = await response.Content.ReadAsStringAsync();
                var orderindexlist = JsonConvert.DeserializeObject<List<OrderSyncPhone>>(answer);

                OrderHeader objOrderHeader = new OrderHeader();
                DataService dataService = new DataService();

                foreach (var item in orderindexlist)
                {
                    //consulto orden
                    objOrderHeader = dataService.Find<OrderHeader>(item.SalesOrderHeaderPhoneId, false);

                    objOrderHeader.SalesOrderHeaderInterId = item.SalesOrderHeaderInterId;
                    objOrderHeader.OrderNum = item.OrderNum;
                    objOrderHeader.TaxAmt = item.TaxAmt;
                    objOrderHeader.Total = item.Total;
                    objOrderHeader.IsSync = true;
                    dataService.Update(objOrderHeader);

                    //Si la orden que se sincronizo, se iba a borrar, la elimino de una vez.
                    if (objOrderHeader.RowMod == "D")
                    {
                        dataService.Delete(objOrderHeader);
                    }
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = "Se sincronizaron correctamente las ordenes."
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> RefreshOrders(
        string urlBase,
        string controler,
        List<OrderHeader> orders)
        {
            try
            {
                string controler2 = String.Empty;
                var syncHeaders = new List<SynOrderHeaderRequest>();
                foreach (var order in orders)
                {
                    controler2 = controler + "/" + order.SalesOrderHeaderInterId;
                    var response = await GetList<OrderSyncPhone>(urlBase, controler2);
                    if (!response.IsSuccess)
                    {
                        // Message = response.Message;
                    }
                    List<OrderSyncPhone> ListOrderSyncPhone = new List<OrderSyncPhone>();
                    ListOrderSyncPhone = (List<OrderSyncPhone>)response.Result;


                    foreach (var item in ListOrderSyncPhone)
                    {
                        OrderHeader objOrderHeader = new OrderHeader();
                        DataService dataService2 = new DataService();
                        objOrderHeader = dataService2.Find<OrderHeader>(order.SalesOrderHeaderId, false);
                        objOrderHeader.SalesOrderHeaderInterId = item.SalesOrderHeaderInterId;
                        objOrderHeader.TaxAmt = item.TaxAmt;
                        objOrderHeader.Total = item.Total;
                        objOrderHeader.OrderNum = item.OrderNum;
                        dataService2.Update(objOrderHeader);
                    }
                }

                return new Response
                {
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }


        public async Task<Response> DeleteOrder(
          string urlBase,
          string controller,
          OrderHeader order
          )
        {
            try
            {

                var body = JsonConvert.SerializeObject(order);
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var response = await client.DeleteAsync(controller);
                var result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<Response>(result);
                    return new Response
                    {
                        IsSuccess = false,
                        Message = error.Message,
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = "Se elimino con exito la orden",
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
    }
}