<?php
include_once "cors.php";
$id = $_GET["userID"];
include_once "funciones.php";
$reportes = obtenerReportesPorUsuario($id);
echo json_encode($reportes);