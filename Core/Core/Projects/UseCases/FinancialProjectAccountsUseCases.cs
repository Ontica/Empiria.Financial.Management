/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                           Component : Use Cases Layer                       *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Use cases                             *
*  Type     : FinancialProjectAccountsUseCases             License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides use cases for update and retrieve accounts for financial projects.                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Financial.Adapters;

namespace Empiria.Financial.Projects.UseCases {

  /// <summary>Provides use cases for update and retrieve accounts for financial projects.</summary>
  public class FinancialProjectAccountsUseCases : UseCase {

    #region Constructors and parsers

    protected FinancialProjectAccountsUseCases() {
      // no-op
    }

    static public FinancialProjectAccountsUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<FinancialProjectAccountsUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public FinancialAccountDescriptor AddAccount(FinancialAccountFields fields) {
      Assertion.Require(fields, nameof(fields));

      var project = FinancialProject.Parse(fields.ProjectUID);

      FinancialAccount account = project.AddAccount(fields);

      project.Save();

      return FinancialAccountMapper.MapToDescriptor(account);
    }


    public FinancialAccountDto GetAccount(FinancialAccountFields fields) {
      Assertion.Require(fields, nameof(fields));

      var project = FinancialProject.Parse(fields.ProjectUID);

      FinancialAccount account = project.GetAccount(fields.UID);

      return FinancialAccountMapper.Map(account);
    }


    public FixedList<NamedEntityDto> GetFinancialAccountTypes(string financialProjectUID) {

      var accountTypes = FinancialAccountType.GetList()
                                             .FindAll(x => x.PlaysRole(FinancialProject.PROJECT_BASE_ACCOUNTS_ROLE));

      return accountTypes.MapToNamedEntityList();
    }


    public FixedList<NamedEntityDto> GetStandardAccounts(string financialProjectUID) {
      Assertion.Require(financialProjectUID, nameof(financialProjectUID));

      var project = FinancialProject.Parse(financialProjectUID);

      FixedList<StandardAccount> stdAccounts = project.GetStandardAccounts();

      return stdAccounts.Select(x => FinancialAccountMapper.MapStdAccountToDto(x))
                        .ToFixedList();
    }


    public FinancialAccountDescriptor RemoveAccount(FinancialAccountFields fields) {
      Assertion.Require(fields, nameof(fields));

      var project = FinancialProject.Parse(fields.ProjectUID);

      FinancialAccount account = project.GetAccount(fields.UID);

      project.RemoveAccount(account);

      project.Save();

      return FinancialAccountMapper.MapToDescriptor(account);
    }


    public FinancialAccountDescriptor UpdateAccount(FinancialAccountFields fields) {
      Assertion.Require(fields, nameof(fields));

      Assertion.Require(fields, nameof(fields));

      var project = FinancialProject.Parse(fields.ProjectUID);

      FinancialAccount account = project.GetAccount(fields.UID);

      project.UpdateAccount(account, fields);

      project.Save();

      return FinancialAccountMapper.MapToDescriptor(account);
    }

    #endregion Use cases

  }  // class FinancialProjectAccountsUseCases

}  // namespace Empiria.Financial.Projects.UseCases
