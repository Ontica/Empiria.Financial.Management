/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                         Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Input Query DTO                         *
*  Type     : ChartOfAccountsQuery                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Query DTO used to search chart of accounts.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.Financial.Adapters {

  /// <summary>Query DTO used to search accounts chart of accounts.</summary>
  public class ChartOfAccountsQuery {

    public string ChartOfAccountsUID {
      get; set;
    } = string.Empty;


    public AccountRoleType RoleType {
      get; set;
    } = AccountRoleType.Undefined;


    public DebtorCreditorType DebtorCreditorType {
      get; set;
    } = DebtorCreditorType.Undefined;


    public string FromAccount {
      get; set;
    } = string.Empty;


    public string ToAccount {
      get; set;
    } = string.Empty;


    public int Level {
      get; set;
    } = 0;


    public string Keywords {
      get; set;
    } = string.Empty;


    public EntityStatus Status {
      get; set;
    } = EntityStatus.All;


    public string OrderBy {
      get; set;
    } = string.Empty;

  }  // class ChartOfAccountsQuery

}  // namespace Empiria.Financial.Adapters
