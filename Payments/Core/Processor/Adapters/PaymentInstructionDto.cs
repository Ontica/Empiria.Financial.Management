/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Integration interface                   *
*  Type     : PaymentInstructionDto                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO that holds information about a payment instruction to be sent to a payments broker. *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Payments.Processor.Adapters {

    /// <summary>Interface that represents a payment instruction to integrate
    /// Empiria Payment with payments brokers.</summary>
  public interface IPaymentInstruction {
   string Account {
    get; set;
   }

   DateTime RequestedDate {
    get; set;
   }

   DateTime DueDate {
     get; set;
    }

    decimal Total {
      get; set;
    }

    string Reference {
      get; set;
    }

   string Description {
     get; set;
    }

    }  // interface IPaymentInstruction



    /// <summary>Output DTO that holds information about a payment instruction
    /// to be sent to a payments broker.</summary>
  public class PaymentInstructionDto : IPaymentInstruction {
   public string Account {
    get; set;
   }

   public DateTime RequestedDate {
     get; set;
   }

  public DateTime DueDate {
    get; set;
  }

  public decimal Total {
    get; set;
  }

  public string Reference {
    get; set;
  }

  public string Description {
    get; set;
  }

  }  // class PaymentInstructionDto

}  // namespace Empiria.Payments.Processor.Adapters
