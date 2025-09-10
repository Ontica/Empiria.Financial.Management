/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                         Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Mapping class                           *
*  Type     : FinancialAccountMapper                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for financial accounts.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents;
using Empiria.History;
using Empiria.StateEnums;

using Empiria.Financial.Projects.Adapters;

namespace Empiria.Financial.Adapters {

  /// <summary> Mapping methods for financial accounts.</summary>
  static public class FinancialAccountMapper {

    #region Methods

    static public FixedList<FinancialAccountDto> Map(FixedList<FinancialAccount> accounts) {
      return accounts.Select(x => Map(x))
                      .ToFixedList();
    }


    static public FinancialAccountDto Map(FinancialAccount account) {
      return new FinancialAccountDto {
        UID = account.UID,
        AccountNo = account.Code,
        FinancialAccountType = account.FinancialAccountType.MapToNamedEntity(),
        Description = account.Description,
        StandardAccount = MapStdAccountToDto(account.StandardAccount),
        Project = account.Project.MapToNamedEntity(),
        OrganizationalUnit = account.OrganizationalUnit.MapToNamedEntity(),
        Currency = account.Currency.MapToNamedEntity(),
        SubledgerAccountNo = account.SubledgerAccountNo,
        Attributes = account.Attributes,
        FinancialData = account.FinancialData,
        StartDate = account.StartDate,
        EndDate = account.EndDate,
        Parent = account.Parent.MapToNamedEntity(),
        Status = account.Status.MapToDto()
      };
    }


    static internal FinancialAccountDto Map(ICreditAccountData account) {
      return new FinancialAccountDto {
        AccountNo = account.AccountNo,
        SubledgerAccountNo = account.SubledgerAccountNo,
        Description = account.CustomerName
      };
    }


    static internal FixedList<FinancialAccountDescriptor> MapToDescriptor(FixedList<FinancialAccount> accounts) {
      return accounts.Select(x => MapToDescriptor(x))
                     .ToFixedList();
    }


    static internal FinancialAccountDescriptor MapToDescriptor(FinancialAccount account) {
      return new FinancialAccountDescriptor {
        UID = account.UID,
        AccountNo = account.Code,
        FinancialAccountTypeName = account.FinancialAccountType.DisplayName,
        Description = account.Description,
        StandardAccountName = MapStdAccountToDto(account.StandardAccount).Name,
        ProjectUID = account.Project.UID,
        ProjectNo = account.Project.ProjectNo,
        ProjectName = account.Project.Name,
        ProjectCategoryName = account.Project.Category.Name,
        OrganizationalUnitName = account.OrganizationalUnit.Name,
        CurrencyName = account.Currency.ISOCode,
        SubledgerAccountNo = account.SubledgerAccountNo,
        Attributes = account.Attributes,
        FinancialData = account.FinancialData,
        StartDate = account.StartDate,
        EndDate = account.EndDate,
        StatusName = account.Status.GetName(),
      };
    }


    static public NamedEntityDto MapStdAccountToDto(StandardAccount stdAccount) {
      return new NamedEntityDto(stdAccount.UID, $"({stdAccount.StdAcctNo}) {stdAccount.FullName}");
    }


    static internal FinancialAccountHolderDto MapToHolderDto(FinancialAccount account) {
      return new FinancialAccountHolderDto {
        Account = Map(account),
        Project = FinancialProjectMapper.MapProject(account.Project),
        OperationAccounts = OperationAccountMapper.MapOperationsAccounts(account),
        Documents = DocumentServices.GetAllEntityDocuments(account),
        History = HistoryServices.GetEntityHistory(account),
        Actions = MapActions(account)
      };
    }

    #endregion Methods

    #region Helpers

    static private BaseActions MapActions(FinancialAccount account) {
      return new BaseActions {
        CanDelete = account.Status == EntityStatus.Pending,
        CanEditDocuments = true,
        CanUpdate = true
      };
    }

    #endregion Helpers

  }  // class FinancialAccountMapper

}  // namespace Empiria.Financial.Adapters
