using System;
using System.Drawing;
using System.Threading.Tasks;

namespace MyGIS
{
  /*
    Original
     2011-12-15 【python】緯度経度から標準地域コードの取得
     http://d.hatena.ne.jp/letitride/20111215
  */
    /// <summary>
    /// 標準地域メッシュを格納する内部クラス
    /// </summary>
    public class StandardMesh
    {
        public string meshCode; //メッシュコード
        public double latitude; // メッシュ左下の緯度
        public double longitude; // メッシュ左下の経度
    }

    /// <summary>
    /// public:地域メッシュコードを計算する際に利用する計算用関数などをまとめたユーティリティークラス。
    /// </summary>
    public static class MeshCodeUtility
    {
        /// <summary>
        /// public:StandardMesh:1次メッシュコードの取得を行うメソッド。
        /// </summary>
        /// <param name="lat">緯度</param>
        /// <param name="lon">経度</param>
        /// <returns></returns>
        public static StandardMesh Get1stMesh(double lat, double lon)
        {
            int left_operator = (int)(lat * 15 / 10);
            int right_operator = (int)(lon - 100);
            //左下(南西端)の緯度経度を計算
            double dest_lat = left_operator / 15.0 * 10;
            double dest_lon = right_operator + 100.0;
            StandardMesh sMesh = new StandardMesh()
            {
                meshCode = left_operator.ToString() + right_operator.ToString(),
                latitude = dest_lat,
                longitude = dest_lon
            };

            return sMesh;
        }

        /// <summary>
        /// public:StandardMesh:2次メッシュコードの取得を行うメソッド。
        /// </summary>
        /// <param name="lat">緯度</param>
        /// <param name="lon">経度</param>
        /// <returns></returns>
        public static StandardMesh Get2ndMesh(double lat, double lon)
        {
            //所属する1次メッシュの西南端のlat,lonを取得。
            StandardMesh baseMesh = Get1stMesh(lat, lon);
            //緯度方向算出
            //2次メッシュは緯度方向5分(5/60=0.08333)区切りとなる。
            int left_operator = (int)((lat - baseMesh.latitude) * 100000 / 8333);
            //経度方向算出
            //経度方向7分30秒(7/60+30/60/60=0.11666+0.008333=0.1249))区切りとなる。
            int right_operator = (int)((lon - baseMesh.longitude) * 1000 / 125);
            //左下(南西端)の緯度経度を計算。
            double dest_lat = left_operator * 8333.0 / 100000 + baseMesh.latitude;
            double dest_lon = right_operator * 125.0 / 1000 + baseMesh.longitude;

            StandardMesh sMesh = new StandardMesh()
            {
                meshCode = baseMesh.meshCode + "-" + left_operator.ToString() + right_operator.ToString(),
                latitude = dest_lat,
                longitude = dest_lon
            };

            return sMesh;
        }

        /// <summary>
        /// public:StandardMesh:3次メッシュコードの取得を行うメソッド。
        /// </summary>
        /// <param name="lat">緯度</param>
        /// <param name="lon">経度</param>
        /// <returns></returns>
        public static StandardMesh Get3rdMesh(double lat, double lon)
        {
            //所属する2次メッシュを取得
            StandardMesh baseMesh = Get2ndMesh(lat, lon);
            //緯度方向算出
            //3次メッシュは緯度方向 30秒(30/60/60=0.008333)区切りとなる。
            int left_operator = (int)((lat - baseMesh.latitude) * 1000000 / 8333);
            //経度方向算出
            //経度方向 45秒(45/60/60=0.0125)区切りとなる。
            int right_operator = (int)((lon - baseMesh.longitude) * 10000 / 125);
            //左下(南西端)の緯度経度を計算。
            double dest_lat = left_operator * 8333 / 1000000.0 + baseMesh.latitude;
            double dest_lon = right_operator * 125 / 10000.0 + baseMesh.longitude;

            StandardMesh sMesh = new StandardMesh()
            {
                meshCode = baseMesh.meshCode + "-" + left_operator.ToString() + right_operator.ToString(),
                latitude = dest_lat,
                longitude = dest_lon
            };

            return sMesh;
        }
    }
}