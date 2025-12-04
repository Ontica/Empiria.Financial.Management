/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                          Component : Web Api                               *
*  Assembly : Empiria.Payments.WebApi.dll                  Pattern   : Web api Controller                    *
*  Type     : PaymentInstructionController                 License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API used to retrive payment orders instructions.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Payments.Adapters;

using Empiria.Payments.Processor.Adapters;
using Empiria.Payments.Processor.Services;

namespace Empiria.Payments.WebApi {

  /// <summary>Web API used to retrive and update payment orders and their catalogues.</summary>
  public class PaymentInstructionController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v2/payments-management/payment-instructions/{instructionUID:guid}")]
    public SingleObjectModel GetPaymentInstruction([FromUri] string instructionUID) {

      using (var usecases = PaymentService.ServiceInteractor()) {
        PaymentInstructionHolderDto instruction = usecases.GetPaymentInstruction(instructionUID);

        return new SingleObjectModel(base.Request, instruction);
      }
    }


    [HttpPost]
    [Route("v2/payments-management/payment-orders/search")]
    [Route("v2/payments-management/payment-instructions/search")]
    public CollectionModel SearchPaymentInstructions([FromBody] PaymentOrdersQuery query) {

      using (var services = PaymentService.ServiceInteractor()) {
        FixedList<PaymentOrderDescriptor> instructions = services.SearchPaymentInstructions(query);

        return new CollectionModel(base.Request, instructions);
      }
    }

    #endregion Query web apis

  }  // class PaymentInstructionController

}  // namespace Empiria.Payments.WebApi
