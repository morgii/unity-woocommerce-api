<?php
/*
Plugin Name: Order API
Description: Custom REST API endpoint to return orders for the logged-in customer via JWT.
Version: 1.0
Author: Radioactive Morgii
*/

if (!defined('ABSPATH')) {
    exit; // Exit if accessed directly
}

// Register REST route
add_action('rest_api_init', function() {
    register_rest_route('custom/v1', '/my-orders', [
        'methods' => 'GET',
        'callback' => function(WP_REST_Request $request) {
            $user_id = get_current_user_id();

            if (!$user_id) {
                return new WP_Error('not_logged_in', 'You must be logged in to view your orders.', ['status' => 401]);
            }

            $orders = wc_get_orders([
                'customer_id' => $user_id,
                'limit' => -1,
                'orderby' => 'date',
                'order' => 'DESC'
            ]);

            $data = [];
            foreach ($orders as $order) {
                $items = [];
                foreach ($order->get_items() as $item) {
                    $items[] = [
                        'id' => $item->get_id(),
                        'name' => $item->get_name(),
                        'product_id' => $item->get_product_id(),
                        'quantity' => $item->get_quantity(),
                        'total' => $item->get_total()
                    ];
                }

                $data[] = [
                    'id' => $order->get_id(),
                    'status' => $order->get_status(),
                    'total' => $order->get_total(),
                    'date_created' => $order->get_date_created()->date('Y-m-d H:i:s'),
                    'line_items' => $items
                ];
            }

            return rest_ensure_response($data);
        },
        'permission_callback' => function() {
            return is_user_logged_in();
        }
    ]);
});
