/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Accounts Chart                             Component : Interface adapters                      *
*  Assembly : FinancialAccounting.Core.dll               Pattern   : Mapper class                            *
*  Type     : AccountsChartMapper                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for accounts charts and their contents.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial.Adapters {

  /// <summary>Mapping methods for accounts charts and their contents.</summary>
  static public class ChartOfAccountsMapper {

    static internal ChartOfAccountsDto Map(StandardAccountsCatalogue catalogue) {
      return new ChartOfAccountsDto {
        UID = catalogue.UID,
        Name = catalogue.Name,
        Accounts = MapToStdAccountDescriptor(catalogue.GetStandardAccounts())
      };
    }

    static private StandardAccountDescriptor MapToStdAccountDescriptor(StandardAccount account) {
      return new StandardAccountDescriptor {
        UID = account.UID,
        Name = account.Name,
        FullName = account.FullName,
        Number = account.StdAcctNo,
        TypeName = account.Category.Name,
        RoleType = account.RoleType,
        DebtorCreditorType = account.DebtorCreditorType,
        Level = account.Level,
        IsLastLevel = false, // account.IsLastLevel,
        StartDate = account.StartDate,
        EndDate = account.EndDate,
        Obsolete = false,
      };
    }

    static private FixedList<StandardAccountDescriptor> MapToStdAccountDescriptor(FixedList<StandardAccount> list) {
      return list.Select((x) => MapToStdAccountDescriptor(x))
                 .ToFixedList();
    }


  }  // class AccountsChartMapper

}  // namespace Empiria.FinancialAccounting.Adapters
