using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class ModeloCarroDA : IModeloCarroDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        #region Constructor
        public ModeloCarroDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }
        #endregion

        #region Operaciones
        public async Task<IEnumerable<ModeloCarroResponse>> ObtenerPorMarca(Guid IdMarca)
        {
            string query = @"ObtenerModelosPorMarca";

            var resultadoConsulta = await _sqlConnection.QueryAsync<ModeloCarroResponse>(
                query,
                new { IdMarca = IdMarca }
            );

            return resultadoConsulta;
        }
        #endregion
    }
}
