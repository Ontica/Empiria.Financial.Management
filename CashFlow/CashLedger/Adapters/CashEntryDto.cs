/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.CashFlow.CashLedger.dll            Pattern   : Output DTO                              *
*  Type     : CashEntryDto                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO for a cash ledger transaction entry.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.CashFlow.CashLedger.Adapters {

  /// <summary>Output DTO for a cash ledger transaction entry.</summary>
  public class CashEntryDto : BaseCashEntryDto {

    public NamedEntityDto CashAccount {
      get; internal set;
    }

    internal bool Processed {
      get; set;
    }

  }  // class CashEntryDto

}  // namespace Empiria.CashFlow.CashLedger.Adapters
