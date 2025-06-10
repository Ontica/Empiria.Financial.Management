/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                         Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Mapping class                           *
*  Type     : FinancialAccountMapper                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for financial accounts.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.Financial.Adapters {

  /// <summary> Mapping methods for financial accounts.</summary>
  static public class FinancialAccountMapper {

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
        Attributes = account.Attributes,
        FinancialData = account.FinancialData,
        StartDate = account.StartDate,
        EndDate = account.EndDate,
        Parent = account.Parent.MapToNamedEntity(),
        Status = account.Status.MapToDto()
      };
    }


    static internal FinancialAccountOperationsDto MapAccountOperations(FinancialAccount account) {

      return new FinancialAccountOperationsDto {

        Account = MapToDescriptor(account),

        AvailableOperations = account.GetAvailableOperations()
                                     .MapToNamedEntityList(),

        CurrentOperations = account.GetOperations()
                                   .MapToNamedEntityList()
      };
    }


    static public FixedList<FinancialAccountDescriptor> MapToDescriptor(FixedList<FinancialAccount> accounts) {
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
        OrganizationalUnitName = account.OrganizationalUnit.Name,
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

  }  // class FinancialAccountMapper

}  // namespace Empiria.Financial.Adapters
