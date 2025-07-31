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
  public class CashTransactionUseCases : UseCase {

    #region Constructors and parsers

    protected CashTransactionUseCases() {
      // no-op
    }

    static public CashTransactionUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<CashTransactionUseCases>();
    }


    #endregion Constructors and parsers

    #region Use cases

    public Task<CashTransactionHolderDto> GetTransaction(long id) {
      Assertion.Require(id > 0, nameof(id));

      return CashTransactionData.GetTransaction(id);
    }


    public Task<FixedList<CashTransactionDescriptor>> SearchTransactions(CashLedgerQuery query) {
      Assertion.Require(query, nameof(query));

      return CashTransactionData.SearchTransactions(query);
    }

    #endregion Use cases

  }  // class CashTransactionUseCases

}  // namespace Empiria.CashFlow.CashLedger.UseCases
