/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.Core.dll                  Pattern   : Information holder                      *
*  Type     : CashFlowProjectionCategory                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a cash flow projection category that serves to classify CashFlowProjectionTypes.    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Ontology;
using Empiria.Parties;
using Empiria.StateEnums;

namespace Empiria.CashFlow.Projections {

  /// <summary>Represents a cash flow projection category that serves to
  /// classify CashFlowProjectionTypes.</summary>
  public class CashFlowProjectionCategory : CommonStorage {

    #region Constructors and parsers

    private CashFlowProjectionCategory() {
      // Required by Empiria Framework.
    }


    static public CashFlowProjectionCategory Parse(int id) => ParseId<CashFlowProjectionCategory>(id);

    static public CashFlowProjectionCategory Parse(string uid) => ParseKey<CashFlowProjectionCategory>(uid);

    static public CashFlowProjectionCategory Empty => ParseEmpty<CashFlowProjectionCategory>();

    static public FixedList<CashFlowProjectionCategory> GetList() {
      return GetStorageObjects<CashFlowProjectionCategory>()
            .FindAll(x => x.Status != EntityStatus.Deleted);
    }

    #endregion Constructors and parsers

    #region Properties

    public CashFlowProjectionType ProjectionType {
      get {
        if (IsEmptyInstance) {
          return CashFlowProjectionType.Empty;
        }

        int id = ExtData.Get<int>("projectionTypeId");

        return ObjectTypeInfo.Parse<CashFlowProjectionType>(id);
      }
    }


    public EntityStatus Status {
      get {
        return base.GetStatus<EntityStatus>();
      }
    }


    public FixedList<OperationSource> OperationSources {
      get {
        return ExtData.GetList<OperationSource>("sources", false)
                      .ToFixedList();
      }
    }

    #endregion Properties

  }  // class CashFlowProjectionCategory

}  // namespace Empiria.CashFlow.Projections
