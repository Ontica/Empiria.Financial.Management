/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts milestone Management             Component : Data Layer                              *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Data service                            *
*  Type     : ContractMilestoneItemData                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for contract milestone item instances.                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

using Empiria.Data;

namespace Empiria.Contracts.Data {

  /// <summary>Provides data read and write methods for contract milestone item instances.</summary>
  static public class ContractMilestoneItemData {

    static internal List<ContractMilestoneItem> GetContractMilestoneItems(ContractMilestone milestone) {
      Assertion.Require(milestone, nameof(milestone));

      var sql = "select * from fms_milestone_items " +
                $"WHERE milestone_id = {milestone.Id} AND milestone_item_status <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList<ContractMilestoneItem>(op);
    }


    static public void WriteMilestoneItem(ContractMilestoneItem o, string extensionData) {
      var op = DataOperation.Parse("write_Milestone_Item",
                     o.Id, o.UID, o.Milestone.Id, o.ContractItem.Id, o.Product.Id, o.Quantity,
                     o.UnitPrice, extensionData, o.Keywords, o.LastUpdatedBy.Id, o.PostingTime,
                     (char) o.Status);

      DataWriter.Execute(op);
    }

  }  // class ContractMilestoneItemData

}  // namespace Empiria.Contracts.Data
