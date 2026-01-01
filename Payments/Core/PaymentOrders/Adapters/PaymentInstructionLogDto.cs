/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Data Transfer Object                    *
*  Type     : PaymentInstructionLogdDescriptorDto        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer objects used to return payment instruction log.                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Payments.Adapters {

  /// <summary>Data transfer objects used to return payment instruction log.</summary>
  public class PaymentInstructionLogDescriptorDto {

    public string UID {
      get; internal set;
    }

    public string PaymentOrdeNo {
      get; internal set;
    }

    public string PaymentMethod {
      get; internal set;
    }

    public string Currency {
      get; internal set;
    }

    public decimal Total {
      get; internal set;
    }

    public DateTime RequestTime {
      get; set;
    }

    public string Description {
      get; internal set;
    }

    public string StatusName {
      get; internal set;
    }

  } // class PaymentInstructionLogDescriptorDto

}  // namespace Empiria.Payments.Adapters
