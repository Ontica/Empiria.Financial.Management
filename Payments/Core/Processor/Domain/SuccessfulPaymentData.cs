/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Information holder                      *
*  Type     : SuccessfulPaymentData                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds data about a successful payment.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Payments.Processor.Adapters;

namespace Empiria.Payments.Processor {

  /// <summary>Holds data about a successful payment.</summary>
  internal class SuccessfulPaymentData {
    public SuccessfulPaymentData(IPaymentResult paymentResult) {
    }
  }  // class SuccessfulPaymentData

}  // namespace Empiria.Payments.Processor
