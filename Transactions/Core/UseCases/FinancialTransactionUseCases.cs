/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Transactions                     Component : Use Cases Layer                         *
*  Assembly : Empiria.Financial.Transactions.Core.dll    Pattern   : Use Case interactor                     *
*  Type     : FinancialTransactionUseCases               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Financial transaction use case interactor.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Threading.Tasks;

using Empiria.Services;

using Empiria.FinancialAccounting.ClientServices;

using Empiria.Financial.Transactions.Adapters;

namespace Empiria.Financial.Transactions.UseCases {

  /// <summary>Financial transaction use case interactor.</summary>
  public class FinancialTransactionUseCases : UseCase {

    private readonly AccountingTransactionServices _accountingServices = new AccountingTransactionServices();

    #region Constructors and parsers

    protected FinancialTransactionUseCases() {
      // no-op
    }

    static public FinancialTransactionUseCases UseCaseInteractor() {
      return CreateInstance<FinancialTransactionUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public async Task<int> ProcessTransaction(FinancialTransaction transaction) {
      Assertion.Require(transaction, nameof(transaction));

      var transactionDto = FinancialTransactionMapper.Map(transaction);

      int notificationId = await _accountingServices.NotifyTransaction(transactionDto);

      return notificationId;
    }

    #endregion Use cases

  }  // class FinancialTransactionUseCases

}  // namespace Empiria.Financial.Transactions.UseCases
