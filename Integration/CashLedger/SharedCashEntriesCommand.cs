/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Integration Adapters Layer              *
*  Assembly : Empiria.Financial.Integration.dll          Pattern   : Command DTO                             *
*  Type     : SharedCashEntriesCommand                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Input command used to update cash transaction entries.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial.Integration.CashLedger {

  /// <summary>Input command used to update cash transaction entries.</summary>
  public class SharedCashEntriesCommand {

    public long[] Entries {
      get; set;
    } = new long[0];


    public int CashAccountId {
      get; set;
    }

  }  // SharedCashEntriesCommand

}  // namespace Empiria.Financial.Integration.CashLedger
