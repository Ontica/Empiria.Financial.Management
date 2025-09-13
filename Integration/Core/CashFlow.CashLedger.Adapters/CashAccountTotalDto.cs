/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                   Component : Integration Adapters Layer           *
*  Assembly : Empiria.Financial.Integration.Core.dll        Pattern   : Output DTO                           *
*  Type     : CashAccountTotalDto                           License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Output DTO for a cash account with totals.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.CashFlow.CashLedger.Adapters {

  /// <summary>Output DTO for a cash account with totals.</summary>
  public class CashAccountTotalDto {

    public string UID {
      get; set;
    }

    public string AccountNo {
      get; set;
    }

    public string AccountName {
      get; set;
    }

    public int CashAccountId {
      get; set;
    }

    public string CashAccountNo {
      get; set;
    }

    public string CashAccountName {
      get; set;
    }

    public string CurrencyCode {
      get; set;
    }

    public decimal Debit {
      get; set;
    }

    public decimal Credit {
      get; set;
    }

  }  // class CashAccountTotalDto

} // namespace Empiria.CashFlow.CashLedger.Adapters
