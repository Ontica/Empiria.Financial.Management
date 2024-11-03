﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adpaters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Integration interface                   *
*  Type     : IPaymentsBroker                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Interface to integrate payment brokers with Empiria Payments.                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments.Processor.Adapters {

  /// <summary>Interface to integrate payment brokers with Empiria Payments.</summary>
  internal interface IPaymentsBroker {

    string UID {
      get;
    }

    IPaymentResult Pay(IPaymentInstruction instruction);

  }  // interface IPaymentsBroker

}  // namespace Empiria.Payments.Processor.Adapters