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

    public FinancialAccountOperationsDto AddAccountOperation(string accountUID, string stdAccountUID) {
      Assertion.Require(accountUID, nameof(accountUID));
      Assertion.Require(stdAccountUID, nameof(stdAccountUID));

      FinancialAccount account = FinancialAccount.Parse(accountUID);
      StandardAccount stdAccount = StandardAccount.Parse(stdAccountUID);

      FinancialAccount operation = account.AddOperation(stdAccount);

      operation.Save();

      return FinancialAccountMapper.MapAccountOperations(account);

    }


    public FinancialAccountOperationsDto GetAccountOperations(string accountUID) {
      Assertion.Require(accountUID, nameof(accountUID));

      FinancialAccount account = FinancialAccount.Parse(accountUID);

      return FinancialAccountMapper.MapAccountOperations(account);
    }


    public FinancialAccountOperationsDto GetFinancialProjectAccountOperations(FinancialAccountFields fields) {
      Assertion.Require(fields, nameof(fields));

      var project = FinancialProject.Parse(fields.ProjectUID);

      FinancialAccount account = project.GetAccount(fields.UID);

      return FinancialAccountMapper.MapAccountOperations(account);
    }


    public FinancialAccountOperationsDto RemoveAccountOperation(string accountUID,
                                                                string operationAccountUID) {
      Assertion.Require(accountUID, nameof(accountUID));
      Assertion.Require(operationAccountUID, nameof(operationAccountUID));

      FinancialAccount account = FinancialAccount.Parse(accountUID);

      FinancialAccount operation = account.RemoveOperation(operationAccountUID);

      operation.Save();

      return FinancialAccountMapper.MapAccountOperations(account);
    }

    #endregion Use cases

  }  // class OperationAccountUseCases

}  // namespace Empiria.Financial.Accounts.UseCases
