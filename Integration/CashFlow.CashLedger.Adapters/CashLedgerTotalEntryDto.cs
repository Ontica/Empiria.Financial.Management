/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Integration.dll          Pattern   : Output DTO                              *
*  Type     : CashLedgerTotalEntryDto                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to retrieve cash ledger totals.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.CashFlow.CashLedger.Adapters {

  /// <summary>Output DTO used to retrieve cash ledger totals.</summary>
  public class CashLedgerTotalEntryDto {

    public int CashAccountId {
      get; set;
    }

    public string CashAccountNo {
      get; set;
    }

    public int CurrencyId {
      get; set;
    }

    public decimal Debit {
      get; set;
    }

    public decimal Credit {
      get; set;
    }

  }  // class CashLedgerTotalEntryDto

}  // namespace Empiria.CashFlow.CashLedger.Adapters
