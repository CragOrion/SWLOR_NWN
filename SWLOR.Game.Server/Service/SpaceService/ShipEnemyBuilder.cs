﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWLOR.Game.Server.Service.SpaceService
{
    public class ShipEnemyBuilder
    {
        private readonly Dictionary<string, ShipEnemyDetail> _shipEnemies = new Dictionary<string, ShipEnemyDetail>();
        private ShipEnemyDetail _activeShipEnemy;

        /// <summary>
        /// Creates a new ship enemy.
        /// </summary>
        /// <param name="creatureTag">The tag of the creature to associate with this ship detail.</param>
        /// <returns>A ship enemy builder with the configured options.</returns>
        public ShipEnemyBuilder Create(string creatureTag)
        {
            _activeShipEnemy = new ShipEnemyDetail();
            _shipEnemies[creatureTag] = _activeShipEnemy;

            return this;
        }

        /// <summary>
        /// Sets the shield of the ship currently being built.
        /// </summary>
        /// <param name="shield">The shield level to set.</param>
        /// <returns>A ship enemy builder with the configured options.</returns>
        public ShipEnemyBuilder Shield(int shield)
        {
            _activeShipEnemy.Shield = shield;

            return this;
        }

        /// <summary>
        /// Sets the hull of the ship currently being built.
        /// </summary>
        /// <param name="hull">The hull level to set.</param>
        /// <returns>A ship enemy builder with the configured options.</returns>
        public ShipEnemyBuilder Hull(int hull)
        {
            _activeShipEnemy.Hull = hull;

            return this;
        }

        /// <summary>
        /// Sets the capacitor of the ship currently being built.
        /// </summary>
        /// <param name="capacitor">The capacitor level to set.</param>
        /// <returns>A ship enemy builder with the configured options.</returns>
        public ShipEnemyBuilder Capacitor(int capacitor)
        {
            _activeShipEnemy.Capacitor = capacitor;

            return this;
        }

        /// <summary>
        /// Sets the accuracy of the ship currently being built.
        /// </summary>
        /// <param name="accuracy">The accuracy to set.</param>
        /// <returns>A ship enemy builder with the configured options.</returns>
        public ShipEnemyBuilder Accuracy(int accuracy)
        {
            _activeShipEnemy.Accuracy = accuracy;

            return this;
        }

        /// <summary>
        /// Sets the evasion of the ship currently being built.
        /// </summary>
        /// <param name="evasion">The evasion to set.</param>
        /// <returns>A ship enemy builder with the configured options.</returns>
        public ShipEnemyBuilder Evasion(int evasion)
        {
            _activeShipEnemy.Evasion = evasion;

            return this;
        }

        /// <summary>
        /// Sets the EM Defense of the ship currently being built.
        /// </summary>
        /// <param name="emDefense">The EM Defense level to set.</param>
        /// <returns>A ship enemy builder with the configured options.</returns>
        public ShipEnemyBuilder EMDefense(int emDefense)
        {
            _activeShipEnemy.EMDefense = emDefense;

            return this;
        }

        /// <summary>
        /// Sets the Explosive Defense of the ship currently being built.
        /// </summary>
        /// <param name="explosiveDefense">The Explosive Defense level to set.</param>
        /// <returns>A ship enemy builder with the configured options.</returns>
        public ShipEnemyBuilder ExplosiveDefense(int explosiveDefense)
        {
            _activeShipEnemy.ExplosiveDefense = explosiveDefense;

            return this;
        }

        /// <summary>
        /// Sets the Thermal Defense of the ship currently being built.
        /// </summary>
        /// <param name="thermalDefense">The Thermal Defense level to set.</param>
        /// <returns>A ship enemy builder with the configured options.</returns>
        public ShipEnemyBuilder ThermalDefense(int thermalDefense)
        {
            _activeShipEnemy.ThermalDefense = thermalDefense;

            return this;
        }

        public Dictionary<string, ShipEnemyDetail> Build()
        {
            return _shipEnemies;
        }
    }
}
