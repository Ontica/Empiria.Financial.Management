/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Domain Layer                            *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Infomation Holder                       *
*  Type     : ContractMilestoneItem                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a contract milestone item.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Contracts {

  /// <summary>Represents a contract milestone item.</summary>
  public class ContractMilestoneItem : BaseObject {

    #region Constructors and parsers

    static internal ContractMilestoneItem Parse(int id) {
      return ParseId<ContractMilestoneItem>(id);
    }

    static internal ContractMilestoneItem Parse(string uid) {
      return ParseKey<ContractMilestoneItem>(uid);
    }

    static internal FixedList<ContractMilestoneItem> GetList() {
      return GetList<ContractMilestoneItem>().ToFixedList();
    }

    static internal ContractMilestoneItem Empty => ParseEmpty<ContractMilestoneItem>();

    #endregion Constructors and parsers

  }  // class ContractMilestoneItem

}  // namespace Empiria.Contracts
