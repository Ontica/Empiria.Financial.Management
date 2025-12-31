/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                         Component : Interface adapters                      *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Mapper                                  *
*  Type     : StandardAccountMapper                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for standard accounts.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.Financial.Adapters {

  /// <summary>Mapping methods for standard accounts.</summary>
  static public class StandardAccountMapper {

    static internal StandardAccountHolder Map(StandardAccount stdAccount) {

      FixedList<FinancialAccount> accounts;

      if (stdAccount.StandardAccountType.IsOperationRelated) {
        accounts = stdAccount.GetNonOperationAccounts();
      } else {
        accounts = stdAccount.GetLeafAccounts();
      }

      return new StandardAccountHolder {
        StandardAccount = MapStdAccount(stdAccount),
        Accounts = FinancialAccountMapper.MapToDescriptor(accounts),
        StandardAccountTypes = stdAccount.ChartOfAccounts.FinancialAccountTypes.MapToNamedEntityList(),
        Actions = MapActions(stdAccount)
      };
    }


    static internal FixedList<StandardAccountDescriptor> MapToDescriptor(FixedList<StandardAccount> list) {
      return list.Select((x) => MapToDescriptor(x))
                 .ToFixedList();
    }


    static internal StandardAccountDescriptor MapToDescriptor(StandardAccount account) {
      return new StandardAccountDescriptor {
        UID = account.UID,
        Description = account.Description,
        FullName = account.FullName,
        Number = account.StdAcctNo,
        TypeName = account.Category.Name,
        RoleType = account.RoleType,
        DebtorCreditorType = account.DebtorCreditorType,
        Level = account.Level,
        IsLastLevel = account.IsLastLevel,
        StatusName = account.Status.GetName(),
        StartDate = account.StartDate,
        EndDate = account.EndDate,
        Obsolete = false,
      };
    }

    #region Helpers

    static private StandardAccountActions MapActions(StandardAccount stdAccount) {
      return new StandardAccountActions {
        CanActivate = stdAccount.Status == EntityStatus.Suspended,
        CanSuspend = stdAccount.Status == EntityStatus.Active,
        CanUpdate = true,
        ShowOperations = stdAccount.StandardAccountType.IsOperationRelated,
        CanEditOperations = stdAccount.StandardAccountType.IsOperationRelated
      };
    }


    static private StandardAccountDto MapStdAccount(StandardAccount account) {
      return new StandardAccountDto {
        UID = account.UID,
        Description = account.Description,
        FullName = account.FullName,
        Number = account.StdAcctNo,
        Type = account.Category.MapToNamedEntity(),
        RoleType = account.RoleType.MapToNamedEntity(),
        DebtorCreditorType = account.DebtorCreditorType.MapToNamedEntity(),
        Classification = account.MainClassification.MapToNamedEntity("Sin clasificación asignada"),
        Level = account.Level,
        IsLastLevel = account.IsLastLevel,
        IsProjectRelated = account.StandardAccountType.IsProjectRelated,
        Status = account.Status.MapToDto(),
        StartDate = account.StartDate,
        EndDate = account.EndDate
      };
    }

    #endregion Helpers

  }  // class StandardAccountMapper

}  // namespace Empiria.FinancialAccounting.Adapters
