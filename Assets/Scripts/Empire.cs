using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Empire
{
    public const int MaxCities = 5;
    [SerializeField] List<City> cities;

    public Empire(List<City> cities)
    {
        this.cities = cities;
    }
    public City GetCapitalCity()
    {
        for (int i = 0; i < cities.Count; i++)
        {
            if (cities[i].GetIfIsCapital())
            {
                return cities[i];
            }
        }

        Debug.Log("No hay capital");
        return null;
    }
    public void SetCapitalCity(City newCapital)
    {
        newCapital.SetCityAsCapital();
    }
    public int GetNumberCities()
    {
        return cities.Count;
    }
    public List<City> GetCities()
    {
        return cities;
    }
    public string[] GetCitiesNames()
    {
        List<string> names = new List<string>();
        foreach (City city in cities)
        {
            names.Add(city.GetName());
        }
        return names.ToArray();
    }
    public Vector3Int GetCoord3CityByIndex(int index)
    {
        return cities[index].GetCoord();
    }
    public string[] GetCitiesCoord2string()
    {
        List<string> coords = new List<string>();
        foreach (City city in cities)
        {
            coords.Add(city.GetCoord2String());
        }
        return coords.ToArray();
    }
    public int GetIndexCityByCoord(Vector3 coord)
    {
        for (int i = 0; i < cities.Count; i++)
        {
            if (cities[i].GetCoord() == coord)
            {
                return i;
            }
        }
        return -1;
    }
    public City GetCityByCoord(Vector3Int coord)
    {
        foreach (City city in cities)
        {
            if (city.GetCoord() == coord)
            {
                return city;
            }
        }
        return cities[0];
    }
    public City SelectCityByIndex(int index)
    {
        if (index < cities.Count)
        {
            return cities[index];
        }
        else
        {
            return cities[0];
        }
    }
    public void CreateCity(Vector3Int coords)
    {
        cities.Add(City.NewCity(string.Format("Polis {0}", GetNumberCities()), coords, 4,1,10));
    }
    public Vector3Int SearchCityByCoord(Vector3Int coord)
    {
        for (int i = 0; i < cities.Count; i++)
        {
            if (cities[i].GetCoord() == coord)
            {
                return coord;
            }
        }
        return new Vector3Int(-1, -1, -1);
    }
    
    public static List<City> Starter()
    {
        return new List<City> {
            City.NewCityZero(),
            };
    }

}
