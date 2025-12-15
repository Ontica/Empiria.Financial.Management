/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                          Component : Web Api                               *
*  Assembly : Empiria.Payments.WebApi.dll                  Pattern   : Query web api                         *
*  Type     : PaymentInstructionController                 License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Query web API used to retrieve payment instructions.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Web.Http;

using Empiria.WebApi;

using Empiria.Payments.Adapters;
using Empiria.Payments.UseCases;

namespace Empiria.Payments.WebApi {

  /// <summary>Query web API used to retrieve payment instructions.</summary>
  public class PaymentInstructionController : WebApiController {

    #region Query web apis

    [HttpGet]
    [Route("v2/payments-management/payment-instructions/{instructionUID:guid}")]
    public SingleObjectModel GetPaymentInstruction([FromUri] string instructionUID) {

      using (var usecases = PaymentInstructionUseCases.UseCaseInteractor()) {
        PaymentInstructionHolderDto instruction = usecases.GetPaymentInstruction(instructionUID);

        return new SingleObjectModel(base.Request, instruction);
      }
    }


    [HttpPost]
    [Route("v2/payments-management/payment-orders/search")]  // ToDo: Remove this deprecated route in future versions.
    [Route("v2/payments-management/payment-instructions/search")]
    public CollectionModel SearchPaymentInstructions([FromBody] PaymentOrdersQuery query) {

      using (var services = PaymentInstructionUseCases.UseCaseInteractor()) {
        FixedList<PaymentInstructionDescriptor> instructions = services.SearchPaymentInstructions(query);

        return new CollectionModel(base.Request, instructions);
      }
    }

    #endregion Query web apis

    #region Query web apis

    [HttpPost]
    [Route("v2/payments-management/payment-instructions/{instructionUID:guid}/cancel")]
    public SingleObjectModel Cancel([FromUri] string instructionUID) {

      using (var usecases = PaymentInstructionUseCases.UseCaseInteractor()) {
        PaymentInstructionHolderDto instruction = usecases.Cancel(instructionUID);

        return new SingleObjectModel(base.Request, instruction);
      }
    }


    [HttpPost]
    [Route("v2/payments-management/payment-instructions/{instructionUID:guid}/cancel-payment-request")]
    public SingleObjectModel CancelPaymentRequest([FromUri] string instructionUID) {

      using (var usecases = PaymentInstructionUseCases.UseCaseInteractor()) {
        PaymentInstructionHolderDto instruction = usecases.CancelPaymentRequest(instructionUID);

        return new SingleObjectModel(base.Request, instruction);
      }
    }


    [HttpPost]
    [Route("v2/payments-management/payment-instructions/{instructionUID:guid}/request-payment")]
    public SingleObjectModel RequestPayment([FromUri] string instructionUID) {

      using (var usecases = PaymentInstructionUseCases.UseCaseInteractor()) {
        PaymentInstructionHolderDto instruction = usecases.RequestPayment(instructionUID);

        return new SingleObjectModel(base.Request, instruction);
      }
    }


    [HttpPost]
    [Route("v2/payments-management/payment-instructions/{instructionUID:guid}/suspend")]
    public SingleObjectModel Suspend([FromUri] string instructionUID) {

      using (var usecases = PaymentInstructionUseCases.UseCaseInteractor()) {
        PaymentInstructionHolderDto instruction = usecases.Suspend(instructionUID);

        return new SingleObjectModel(base.Request, instruction);
      }
    }

    #endregion Query web apis

  }  // class PaymentInstructionController

}  // namespace Empiria.Payments.WebApi
