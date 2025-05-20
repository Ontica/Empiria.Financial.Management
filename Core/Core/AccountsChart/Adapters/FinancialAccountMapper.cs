/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                         Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Management.Core.dll      Pattern   : Mapping class                           *
*  Type     : FinancialAccountMapper                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for financial accounts.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial.Accounts.Adapters {

  /// <summary> Mapping methods for financial accounts.</summary>
  static public class FinancialAccountMapper {

    static public FixedList<FinancialAccountDto> Map(FixedList<FinancialAccount> accounts) {
      return accounts.Select(x => Map(x)).ToFixedList();
    }


    static public FinancialAccountDto Map(FinancialAccount account) {
      return new FinancialAccountDto {
        UID = account.UID,
        StandardAccount = account.StandardAccount.MapToNamedEntity(),
        Project = account.Project.MapToNamedEntity(),
        Name = account.Name,
        OrganizationUnit = account.OrganizationUnit.MapToNamedEntity(),
        Parent = account.Parent.MapToNamedEntity(),
        Status = account.Status,
      };
    }

  }  // class FinancialAccountMapper

}  // namespace Empiria.Financial.Accounts.Adapters
