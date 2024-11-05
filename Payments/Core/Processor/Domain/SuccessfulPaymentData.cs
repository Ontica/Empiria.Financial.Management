/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Information holder                      *
*  Type     : SuccessfulPaymentData                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds data about a successful payment.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Payments.Processor.Adapters;

namespace Empiria.Payments.Processor {

  /// <summary>Holds data about a successful payment.</summary>
  internal class SuccessfulPaymentData {

    #region Constructors and Parsers 
    public SuccessfulPaymentData(IPaymentResult paymentResult) {
    }

    #endregion Constructors and Parsers 

    #region Properties

    public decimal Amount {
      get; internal set;
    } = 0;


    public string TrankigCode {
      get; internal set;
    } = string.Empty;


    public string PayerAccountType {
      get; internal set;
    } = string.Empty;


    public string Reference {
      get; internal set;
    } = string.Empty;


    public string Concept {
      get; internal set;
    } = string.Empty;


    public DateTime TimeStamp {
      get; internal set;
    } = DateTime.Today;  

    #endregion Properties


  }  // class SuccessfulPaymentData

}  // namespace Empiria.Payments.Processor
