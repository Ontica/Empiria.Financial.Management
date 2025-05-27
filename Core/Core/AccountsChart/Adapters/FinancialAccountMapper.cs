/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                         Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Management.Core.dll      Pattern   : Mapping class                           *
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

    static public FixedList<FinancialAccountDescriptor> MapToDescriptor(FixedList<FinancialAccount> accounts) {
      return accounts.Select(x => MapToDescriptor(x))
                     .ToFixedList();
    }

    static public FinancialAccountDto Map(FinancialAccount account) {
      return new FinancialAccountDto {
        UID = account.UID,
        AccountNo = account.AccountNo,
        Description = account.Name,
        StandardAccount = account.StandardAccount.MapToNamedEntity(),
        Project = account.Project.MapToNamedEntity(),
        OrganizationalUnit = account.OrganizationalUnit.MapToNamedEntity(),
        StartDate = account.StartDate,
        EndDate = account.EndDate,
        Parent = account.Parent.MapToNamedEntity(),
        Status = account.Status.MapToDto()
      };
    }

    #region Helpers

    static private FinancialAccountDescriptor MapToDescriptor(FinancialAccount account) {
      return new FinancialAccountDescriptor {
         UID = account.UID,
         AccountNo = account.AccountNo,
         Description = account.Description,
         StandardAccountName = account.StandardAccount.Name,
         ProjectName = account.Project.Name,
         OrganizationalUnitName = account.OrganizationalUnit.Name,
         StartDate = account.StartDate,
         EndDate = account.EndDate,
         StatusName = account.Status.GetName()
      };
    }

    #endregion Helpers

  }  // class FinancialAccountMapper

}  // namespace Empiria.Financial.Adapters
