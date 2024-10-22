/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Data Layer                              *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Data service                            *
*  Type     : ContractItemData                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for contract item instances.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

using Empiria.Data;

namespace Empiria.Contracts.Data {

  /// <summary>Provides data read and write methods for contract item instances.</summary>
  static public class ContractItemData {

    static internal List<ContractItem> GetContractItems(Contract contract) {
      Assertion.Require(contract, nameof(contract));

      var sql = "SELECT * FROM FMS_CONTRACT_ITEMS " +
                $"WHERE contract_id = {contract.Id} AND contract_item_status <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList<ContractItem>(op);
    }

    static public void WriteContractItem(ContractItem o, string extensionData) {
      var op = DataOperation.Parse("write_Contract_Item",
                     o.Id, o.UID, o.Contract.Id, o.Product.Id, o.Description,
                     o.UnitMeasure.Id, o.FromQuantity, o.ToQuantity,
                     o.UnitPrice, o.Project.Id, o.PaymentsPeriodicity.Id,
                     o.BudgetAccount.Id, extensionData, o.Keywords,
                     o.LastUpdatedBy.Id, o.PostingTime, (char) o.Status);

      DataWriter.Execute(op);
    }

  }  // class ContractData

}  // namespace Empiria.Contracts.Data
