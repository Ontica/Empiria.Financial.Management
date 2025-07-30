/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash Ledger                                Component : Use cases Layer                         *
*  Assembly : Empiria.CashFlow.CashLedger.dll            Pattern   : Use case interactor class               *
*  Type     : CashLedgerUseCases                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases used to retrive and manage cash ledger transactions.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Threading.Tasks;

using Empiria.Services;

using Empiria.CashFlow.CashLedger.Adapters;
using Empiria.CashFlow.CashLedger.Data;

namespace Empiria.CashFlow.CashLedger.UseCases {

  /// <summary>Use cases used to retrive and manage cash ledger transactions.</summary>
  public class CashLedgerUseCases : UseCase {

    #region Constructors and parsers

    protected CashLedgerUseCases() {
      // no-op
    }

    static public CashLedgerUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<CashLedgerUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public Task<FixedList<NamedEntityDto>> GetAccountingLedgers() {

      return CashLedgerData.GetAccountingLedgers();
    }


    public Task<FixedList<NamedEntityDto>> GetTransactionSources() {

      return CashLedgerData.GetTransactionSources();
    }


    public Task<FixedList<NamedEntityDto>> GetTransactionTypes() {

      return CashLedgerData.GetTransactionTypes();
    }


    public Task<FixedList<NamedEntityDto>> GetVoucherTypes() {

      return CashLedgerData.GetVoucherTypes();
    }


    public Task<FixedList<CashTransactionDescriptor>> SearchTransactions(CashLedgerQuery query) {
      Assertion.Require(query, nameof(query));

      return CashLedgerData.GetTransactions(query);
    }

    #endregion Use cases

  }  // class CashLedgerUseCases

}  // namespace Empiria.CashFlow.CashLedger.UseCases
