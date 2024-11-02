/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Information holder                      *
*  Type     : RejectedPaymentData                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds data about a rejected payment instruction.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Payments.Processor.Adapters;

namespace Empiria.Payments.Processor {

  /// <summary>Holds data about a rejected payment instruction.</summary>
  internal class RejectedPaymentData {
    public RejectedPaymentData(IPaymentResult paymentResult) {
    }
  }  // class RejectedPaymentData

}  // namespace Empiria.Payments.Processor
