<?php
include_once "cors.php";
$newOrder = json_decode(file_get_contents("php://input"));
include_once "funciones.php";
$creacionesPrevias = BuscarCreacionTropa($newOrder);
$subidaDeNivel = BuscarSubidaNivel($newOrder);
if(count($creacionesPrevias) > 0 || count($subidaDeNivel) > 0) {
    $resultado = null;
    echo 'Esta ciudad ya tiene una creacion de tropa en curso o esta subiendo de nivel';
}
else{
    $resultado = crearOrden($newOrder);
}
echo json_encode($resultado);