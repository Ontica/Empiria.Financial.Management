/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Information holder                      *
*  Type     : RejectedPaymentData                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds data about a rejected payment instruction.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Payments.Processor.Adapters;

namespace Empiria.Payments.Processor {

  /// <summary>Holds data about a rejected payment instruction.</summary>
  internal class RejectedPaymentData {

    #region Constructors and Parsers

    public RejectedPaymentData(PaymentResultDto paymentResult) {

    }

    #endregion Constructors and Parsers

    #region Properties

    public string ErrorCode {
      get; internal set;
    } = string.Empty;


    public string ErrorDescription {
      get; internal set;
    } = string.Empty;


    public string ErrorSource {
      get; internal set;
    } = string.Empty;


    public string ErrorStep {
      get; internal set;
    } = string.Empty;


    public string ErrorReason {
      get; internal set;
    } = string.Empty;


    public DateTime TimeStamp {
      get; internal set;
    } = DateTime.Today;

    #endregion Properties

    #region Methods

    #endregion Methods

  }  // class RejectedPaymentData

}  // namespace Empiria.Payments.Processor
