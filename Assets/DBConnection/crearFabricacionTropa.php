<?php
include_once "cors.php";
$newOrder = json_decode(file_get_contents("php://input"));
include_once "funciones.php";
$creacionesPrevias = BuscarCreacionTropa($newOrder);
$subidaDeNivel = BuscarSubidaNivel($newOrder);
if(count($creacionesPrevias) > 0){
    $resultado = 'CreacionEnCurso';
} 
elseif(count($subidaDeNivel) > 0){
    $resultado = 'UpgradeEnCurso';
}
else{
    $resultado = crearOrden($newOrder);
}
echo $resultado;