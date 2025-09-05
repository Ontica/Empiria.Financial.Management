/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                           Component : Use Cases Layer                       *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Use cases                             *
*  Type     : OperationAccountUseCases                     License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Use cases for update and retrieve operation accounts belonging to financial accounts.          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Financial.Projects;

using Empiria.Financial.Adapters;

namespace Empiria.Financial.UseCases {

  /// <summary>Use cases for update and retrieve operation accounts belonging to financial accounts.</summary>
  public class OperationAccountUseCases : UseCase {

    #region Constructors and parsers

    protected OperationAccountUseCases() {
      // no-op
    }

    static public OperationAccountUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<OperationAccountUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public OperationAccountsStructure AddOperationAccount(OperationAccountFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();

      FinancialAccount account = FinancialAccount.Parse(fields.BaseAccountUID);

      FinancialAccount operation = account.AddOperation(fields);

      operation.Save();

      return OperationAccountMapper.MapOperationsAccounts(account);
    }


    public OperationAccountsStructure GetOperationAccounts(string accountUID) {
      Assertion.Require(accountUID, nameof(accountUID));

      FinancialAccount account = FinancialAccount.Parse(accountUID);

      return OperationAccountMapper.MapOperationsAccounts(account);
    }


    public OperationAccountsStructure GetProjectOperationAccounts(FinancialAccountFields fields) {
      Assertion.Require(fields, nameof(fields));

      var project = FinancialProject.Parse(fields.ProjectUID);

      FinancialAccount account = project.GetAccount(fields.UID);

      return OperationAccountMapper.MapOperationsAccounts(account);
    }


    public OperationAccountsStructure RemoveOperationAccount(string accountUID,
                                                             string operationAccountUID) {
      Assertion.Require(accountUID, nameof(accountUID));
      Assertion.Require(operationAccountUID, nameof(operationAccountUID));

      FinancialAccount account = FinancialAccount.Parse(accountUID);

      FinancialAccount operation = account.RemoveOperation(operationAccountUID);

      operation.Save();

      return OperationAccountMapper.MapOperationsAccounts(account);
    }

    #endregion Use cases

  }  // class OperationAccountUseCases

}  // namespace Empiria.Financial.Accounts.UseCases
