/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Output DTO                              *
*  Type     : PaymentInstructionHolderDto                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO with a payment instruction holder.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents;
using Empiria.History;

using Empiria.Billing.Adapters;

namespace Empiria.Payments.Adapters {

  /// <summary>Output DTO with a payment instruction holder.</summary>
  internal class PaymentInstructionHolderDto {

    public PaymentOrderDto PaymentOrder {
      get; internal set;
    }

    public FixedList<PaymentInstructionLogDescriptorDto> Log {
      get; internal set;
    }

    public FixedList<BillDto> Bills {
      get; internal set;
    }


    public FixedList<DocumentDto> Documents {
      get; internal set;
    }

    public FixedList<HistoryEntryDto> History {
      get; internal set;
    }

    public PaymentOrderActionsDto Actions {
      get; internal set;
    }

  } // class PaymentInstructionHolderDto

}  // namespace Empiria.Payments.Processor.Adapters
