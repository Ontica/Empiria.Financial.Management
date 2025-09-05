/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                         Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Mapping class                           *
*  Type     : OperationAccountMapper                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Mapping methods for operation accounts.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial.Adapters {

  /// <summary> Mapping methods for operation accounts.</summary>
  static public class OperationAccountMapper {

    #region Methods

    static internal OperationAccountsStructure MapOperationsAccounts(FinancialAccount baseAccount) {

      return new OperationAccountsStructure {

        BaseAccount = FinancialAccountMapper.MapToDescriptor(baseAccount),

        AvailableOperations = baseAccount.GetAvailableOperations()
                                         .MapToNamedEntityList(),

        CurrentOperations = MapToOperationAccounts(baseAccount.GetOperations())
      };
    }


    #endregion Methods

    #region Helpers

    static private OperationAccountDto MapToOperationAccount(FinancialAccount operationAccount) {
      return new OperationAccountDto {
        UID = operationAccount.UID,
        AccountNo = operationAccount.AccountNo,
        OperationTypeName = operationAccount.StandardAccount.Name,
        CurrencyName = operationAccount.Currency.Name
      };
    }

    static private FixedList<OperationAccountDto> MapToOperationAccounts(FixedList<FinancialAccount> operationAccounts) {
      return operationAccounts.Select(x => MapToOperationAccount(x))
                              .ToFixedList();
    }

    #endregion Helpers

  }  // class OperationAccountMapper

}  // namespace Empiria.Financial.Adapters
