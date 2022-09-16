<?php
include_once "cors.php";
$id = $_GET["orderID"];
include_once "funciones.php";
$order = obtenerOrdenPorID($id);
echo json_encode($order);