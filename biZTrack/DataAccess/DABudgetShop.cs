using System.Collections.Generic;
using biZTrack.Interfaces;
using biZTrack.Models;
using Oracle.ManagedDataAccess.Client;

namespace biZTrack.DataAccess
{
    public class DABudgetShop : IBudgetShop
    {
        public Response GetBudgetShopPriceList()
        {
            Response result = new Response();

            string Query = "SELECT " +
                                "wmv_material_code, " +
                                "wmv_material_description, " +
                                "wmv_unit, " +
                                "wmv_balance_quantity, " +
                                "wmv_selling_price " +
                           "FROM " +
                                "wel_materialdetails_view " +
                           "WHERE " +
                                "wmv_balance_quantity > 0";

            DBconnect.connect();

            OracleDataReader odr = DBconnect.readtable(Query);
            EWODetails res = new EWODetails();
            List<BudgetShopPriceModel> BudgetShopPriceList = new List<BudgetShopPriceModel>();
            result.statusCode = 404;
            while (odr.Read())
            {
                BudgetShopPriceModel BudgetShopPrice = new BudgetShopPriceModel();
                BudgetShopPrice.material_code = odr["wmv_material_code"].ToString();
                BudgetShopPrice.material_description = odr["wmv_material_description"].ToString();
                BudgetShopPrice.unit = odr["wmv_unit"].ToString();
                BudgetShopPrice.balance_quantity = odr["wmv_balance_quantity"].ToString();
                BudgetShopPrice.selling_price = odr["wmv_selling_price"].ToString();
                BudgetShopPriceList.Add(BudgetShopPrice);

                result.statusCode = 200;
            }

            result.resultSet = BudgetShopPriceList;
            DBconnect.disconnect();

            return result;
        }
    }
}
