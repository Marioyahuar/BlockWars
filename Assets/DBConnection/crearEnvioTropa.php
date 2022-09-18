<?php
include_once "cors.php";
$newOrder = json_decode(file_get_contents("php://input"));
include_once "funciones.php";
$TropasParaColonizar = 20; //Podría ser una consulta desde la tabla

if($newOrder->IDTipo_Orden == 4){
    //Ataque
    $ataquesPrevios = BuscarAtaque($newOrder);
    if(count($ataquesPrevios) > 0) {
        $resultado = null;
        echo 'Esta ciudad ya esta siendo atacada en esa ronda';
    }
    else{
        $resultado = crearEnvioTropa($newOrder);
    }
} elseif ($newOrder->IDTipo_Orden == 3){
    //Envío
    $resultado = crearEnvioTropa($newOrder);
} elseif($newOrder->IDTipo_Orden == 5){
    //Colonizacion
    $ciudadDestino = obtenerSpot($newOrder->IDCiudadDestino);
    $ciudadOrigen = obtenerSpot($newOrder->IDCiudadOrigen);
    if($ciudadDestino->TipoSpot != "Valle" || $newOrder->TropasSalida < $TropasParaColonizar){
        echo 'Esta ciudad no es colonizable o no estas enviando tropa suficiente';
        $resultado = null;
    } else{
        $resultado = crearEnvioTropa($newOrder);
    }
}

echo json_encode($resultado);