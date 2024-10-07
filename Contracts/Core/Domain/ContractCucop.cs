/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Domain Layer                            *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Infomation Holder                       *
*  Type     : ContractCucop                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a contract CUCOP.                                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Contracts {

  /// <summary>Represents a contract CUCOP.</summary>
  public class ContractCucop : GeneralObject {

    #region Constructors and parsers

    static internal ContractCucop Parse(int id) {
      return BaseObject.ParseId<ContractCucop>(id);
    }


    static internal ContractCucop Parse(string uid) {
      return BaseObject.ParseKey<ContractCucop>(uid);
    }


    static internal FixedList<ContractCucop> GetList() {
      return BaseObject.GetList<ContractCucop>()
                       .ToFixedList();
    }

    static internal ContractCucop Empty => BaseObject.ParseEmpty<ContractCucop>();

    #endregion Constructors and parsers

  }  // class ContractCucop

}  // namespace Empiria.Contracts
