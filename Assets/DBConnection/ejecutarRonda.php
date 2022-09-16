<?php
include_once "cors.php";
include_once "funciones.php";
$resultado = pasarRonda();
$rondaActual = obtenerRonda();

//Colonizaciones

$ordenesColonizacion = obtenerOrdenesColonizacion($rondaActual);
$totalColonizaciones = count($ordenesColonizacion);

for($g = 0; $g < $totalColonizaciones; $g++){
    $result = EjecutarColonizacion($ordenesColonizacion[$g]);
    $result2 = finalizarOrden($ordenesColonizacion[$g]->IDOrden_en_Curso);
}

//Creacion de tropas
$ordenes = obtenerOrdenesCreacionTropa($rondaActual);
$totalordenes = count($ordenes);

for($i = 0; $i < $totalordenes; $i++){
    $nuevasTropas = new stdClass();
    $nuevasTropas->Tropas = $ordenes[$i]->TropasSalida; 
    $nuevasTropas->IDMundoSpot = $ordenes[$i]->IDCiudadDestino;
    $nuevasTropas->ordenID = $ordenes[$i]->IDOrden_en_Curso;
    $result = agregarTropas($nuevasTropas);
    $result2 = finalizarOrden($nuevasTropas->ordenID);
}

//Subidas de nivel de ciudad
$ordenesUpgradeCity = obtenerOrdenesUpgradeCiudad($rondaActual);
$totalordenesUC = count($ordenesUpgradeCity);

for($j = 0; $j < $totalordenesUC; $j++){
    $result = UpgradeCiudad($ordenesUpgradeCity[$j]->IDCiudadDestino);
    $result2 = finalizarOrden($ordenesUpgradeCity[$j]->IDOrden_en_Curso);
}

//Cálculo de envíos
$ordenesEnvio = obtenerOrdenesEnvioTropa($rondaActual);
$totalordenesEnvio = count($ordenesEnvio);

for($k = 0; $k < $totalordenesEnvio; $k++){
    $tropasEnviadas = new stdClass();
    $tropasEnviadas->Tropas = $ordenesEnvio[$k]->TropasSalida; 
    $tropasEnviadas->IDMundoSpot = $ordenesEnvio[$k]->IDCiudadDestino;
    $tropasEnviadas->ordenID = $ordenesEnvio[$k]->IDOrden_en_Curso;
    $result = agregarTropas($tropasEnviadas);
    $result2 = finalizarOrden($tropasEnviadas->ordenID);
}

//Cálculo de ataque
$ordenesAtaque = obtenerOrdenesAtaque($rondaActual);
$totalordenesAtaque = count($ordenesAtaque);

for($l = 0; $l < $totalordenesAtaque; $l++){
    $atacantes = $ordenesAtaque[$l]->TropasSalida; //Tenemos a los atacantes
    $ciudadDefensora = obtenerSpot($ordenesAtaque[$l]->IDCiudadDestino);
    $defensores = $ciudadDefensora->Tropas; //Tenemos los defensores
    //$resultBatalla = EjecutarBatalla($atacantes,$defensores,$ciudadDefensora->IDMundoSpot);
    $resultBatalla = EjecutarBatalla($atacantes,$defensores,$ordenesAtaque[$l]);
    $result2 = finalizarOrden($ordenesAtaque[$l]->IDOrden_en_Curso);
    //echo json_encode($result2);
}

echo json_encode($rondaActual);
//echo json_encode($result2);

/*
$atacantes = 20;
$ciudadDefensora = ciudad(275);
$defensores = 25;
EjecutarBatalla(20,25,ordenDeataque) => ordenataque tiene todo de la tabla ordenes
*/