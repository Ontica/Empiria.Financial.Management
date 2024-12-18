﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Integration interface                   *
*  Type     : IPaymentResult                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Integration interface that represents a payment result sent by a payments broker.              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments.Processor.Adapters {

    /// <summary>Integration interface that represents a payment result sent by a payments broker.</summary>
    public interface IPaymentResult {

    bool Failed {
      get;
    }

  }  // interface IPaymentResult



   public class PaymentResultDto : IPaymentResult {

    public bool Failed {
      get;
      set;
    }

  }  // class PaymentResultDto

}  // namespace Empiria.Payments.Processor.Adapters
